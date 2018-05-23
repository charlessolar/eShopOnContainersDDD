import Debug from 'debug';
import { History } from 'history';
import { applySnapshot, flow, getEnv, types } from 'mobx-state-tree';
import { BuyerModel, BuyerType } from '../../../models/ordering/buyer';
import { AlertStackType, ApiClientType, AuthenticationType } from '../../../stores';
import { DTOs } from '../../../utils/eShop.dtos';
import { AddressModel, AddressType } from '../models/address';
import { BasketModel, BasketType } from '../models/basket';
import { ItemIndexModel, ItemIndexType } from '../models/items';
import { PaymentMethodModel, PaymentMethodType } from '../models/paymentMethod';

const debug = new Debug('checkout');

export interface CheckoutStoreType {
  loading: boolean;
  basket: BasketType;
  items: Map<string, ItemIndexType>;

  buyer: BuyerType;

  selectedBillingAddress: AddressType;
  selectedShippingAddress: AddressType;

  selectedPaymentMethod: PaymentMethodType;

  orderCompleted: () => void;
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
    const orderCompleted = () => {
      localStorage.removeItem('basket.eShop');

      const alerts = getEnv(self).alertStack as AlertStackType;
      alerts.add('info', 'order placed!');

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

    return { load, orderCompleted, validateBasket, afterCreate, selectBilling, selectShipping, selectPayment };
  });
