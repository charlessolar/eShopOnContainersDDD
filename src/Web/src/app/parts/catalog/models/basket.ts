import { types, getRoot, getEnv, flow, applyAction, applySnapshot, onSnapshot, addDisposer } from 'mobx-state-tree';
import * as validate from 'validate.js';
import { DateTime } from 'luxon';
import uuid from 'uuid/v4';
import Debug from 'debug';

import { models } from '../../../utils';
import { FormatDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { ProductType } from '../../../models/catalog/products';
import { BasketType as BasketTypeBase, BasketModel as BasketModelBase } from '../../../models/basket/baskets';

const debug = new Debug('basket');

export interface BasketType {
  basketId: string;
  totalItems: number;
  basket: BasketTypeBase;
  loading: boolean;
  init: () => Promise<{}>;
  get: () => Promise<{}>;
  addToCart: (product: ProductType) => Promise<{}>;
}
export const BasketModel = types
  .model({
    basketId: types.maybe(types.string),
    totalItems: types.optional(types.number, 0),
    basket: types.maybe(BasketModelBase),
    loading: types.optional(types.boolean, false)
  })
  .actions(self => {

    const init = flow(function*() {
      const client = getEnv(self).api as ApiClientType;

      try {
        const request = new DTOs.InitiateBasket();
        request.basketId = uuid();
        // set beforehand so init is only run once
        self.basketId = request.basketId;

        yield client.command(request);
      } catch (error) {
        self.basketId = undefined;
        debug('received http error: ', error);
        throw error;
      }
    });
    const get = flow(function*() {
      const client = getEnv(self).api as ApiClientType;

      if (!self.basketId) {
        yield init();
        return;
      }

      self.loading = true;
      try {
        const request = new DTOs.GetBasket();
        request.basketId = self.basketId;

        const response: DTOs.QueryResponse<DTOs.Basket> = yield client.query(request);

        applySnapshot(self.basket, response.payload);
      } catch (error) {
        // 'forget' basket
        self.basketId = undefined;
        self.basket = undefined;
        self.totalItems = 0;
        debug('received http error: ', error);
        throw error;
      }
      self.loading = false;
    });
    const addToCart = flow(function*(product: ProductType) {
      const client = getEnv(self).api as ApiClientType;

      try {
        const request = new DTOs.AddBasketItem();
        request.basketId = self.basketId;
        request.productId = product.id;
        yield client.command(request);

        // todo: its possible this command is processed at the exact moment
        // get is refreshing the basket.  Leading to a weird
        // "0 items in cart", "1 item in cart", "0 items in cart"
        // moment
        self.totalItems++;

      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
    });

    // refresh bucket every 10 seconds
    const timerId = setInterval(() => applyAction(self, { name: 'get' }), 10000);

    const afterCreate = () => {
      const basketStorage = localStorage.getItem('basket.eShop');
      applySnapshot(self, basketStorage ? JSON.parse(basketStorage) : {});

      const disposer = onSnapshot(self, state => {
        localStorage.setItem('basket.eShop', JSON.stringify(state));
      });
      addDisposer(self, disposer);

      addDisposer(self, () => {
        clearInterval(timerId);
      });

      if (!self.basketId) {
        init();
      }
    };
    return { init, get, addToCart, afterCreate };
  });
