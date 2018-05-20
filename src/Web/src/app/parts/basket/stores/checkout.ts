import { types, flow, getEnv, getParent, applySnapshot, getSnapshot, onSnapshot, addDisposer } from 'mobx-state-tree';
import { History } from 'history';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType, AlertStackType, AuthenticationType } from '../../../stores';

import { AddressType, AddressModel } from '../models/address';
import { PaymentMethodType, PaymentMethodModel } from '../models/paymentMethod';
import { BasketType, BasketModel } from '../models/basket';
import { ItemIndexType, ItemIndexModel } from '../models/items';

import { BuyerType, BuyerModel } from '../../../models/ordering/buyer';

const debug = new Debug('checkout');

export interface CheckoutStoreType {
  loading: boolean;
  basket: BasketType;
  items: Map<string, ItemIndexType>;

  buyer: BuyerType;

  selectedBillingAddress: AddressType;
  selectedShippingAddress: AddressType;

  selectedPaymentMethod: PaymentMethodType;

  selectBilling: (address: AddressType) => void;
  selectShipping: (address: AddressType) => void;
  selectPayment: (payment: PaymentMethodType) => void;
  load: () => Promise<{}>;
  validateBasket: () => void;
  submit: () => Promise<{}>;
}

export const CheckoutStoreModel = types
  .model('CheckoutStore',
    {
      loading: types.optional(types.boolean, false),

      basketId: types.maybe(types.string),
      basket: types.maybe(BasketModel),
      items: types.optional(types.map(ItemIndexModel), {}),

      buyer: types.maybe(BuyerModel),

      selectedBillingAddress: types.maybe(AddressModel),
      selectedShippingAddress: types.maybe(AddressModel),
      selectedPaymentMethod: types.maybe(PaymentMethodModel)
    })
  .actions(self => {
    const load = flow(function*() {
      debug('loading catalog details');

      const client = getEnv(self).api as ApiClientType;

      self.loading = true;
      try {
        const request = new DTOs.GetBasket();
        request.basketId = self.basketId;
        const basketResponse: DTOs.QueryResponse<DTOs.Basket> = yield client.query(request);

        if (!self.basket) {
          self.basket = BasketModel.create(basketResponse.payload);
        } else {
          applySnapshot(self.basket, basketResponse.payload);
        }

        const claimRequest = new DTOs.ClaimBasket();
        claimRequest.basketId = self.basketId;
        yield client.command(claimRequest);

        const auth = getEnv(self).auth as AuthenticationType;
        self.basket.customer = auth.name;
        self.basket.customerId = auth.username;

        const items = new DTOs.GetBasketItems();
        items.basketId = self.basket.id;
        const itemsResponse: DTOs.PagedResponse<DTOs.BasketItemIndex> = yield client.paged(items);

        self.items.clear();
        itemsResponse.records.forEach(item => {
          self.items.put(item);
        });

        try {
          const buyerRequest = new DTOs.Buyer();
          const buyer: DTOs.QueryResponse<DTOs.OrderingBuyer> = yield client.query(buyerRequest);

          self.buyer = BuyerModel.create(buyer.payload);
        } catch {
          const initBuyer = new DTOs.InitiateBuyer();
          yield client.command(initBuyer);

          self.buyer = BuyerModel.create({ id: auth.username, givenName: auth.name, goodStanding: true });
        }
      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
      self.loading = false;
    });
    const validateBasket = () => {
      if (self.basketId && self.basket.customerId && self.basket.totalItems > 0 && self.items.size > 0) {
        return;
      }

      const alerts = getEnv(self).alertStack as AlertStackType;
      alerts.add('error', 'invalid basket state');

      const history = getEnv(self).history as History;
      history.push('/');
    };

    const afterCreate = () => {
      const basketStorage = localStorage.getItem('basket.eShop');
      applySnapshot(self, basketStorage ? JSON.parse(basketStorage) : {});

    };

    const selectBilling = (address: AddressType) => {
      self.selectedBillingAddress = address;
    };
    const selectShipping = (address: AddressType) => {
      self.selectedShippingAddress = address;
    };
    const selectPayment = (payment: PaymentMethodType) => {
      self.selectedPaymentMethod = payment;
    };

    return { load, validateBasket, afterCreate, selectBilling, selectShipping, selectPayment };
  });
