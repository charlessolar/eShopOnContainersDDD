import { types, flow, getEnv, getParent, applySnapshot, getSnapshot } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { BasketType, BasketModel } from '../models/basket';
import { ItemIndexType, ItemIndexModel } from '../models/items';

const debug = new Debug('checkout');

export interface CheckoutStoreType {
  loading: boolean;
  basket: BasketType;
  items: Map<string, ItemIndexType>;

  get: () => Promise<{}>;
}

export const CheckoutStoreModel = types
  .model('CheckoutStore',
    {
      loading: types.optional(types.boolean, false),

      basket: types.maybe(BasketModel),
      items: types.optional(types.map(ItemIndexModel), {}),

    })
  .actions(self => {
    const get = flow(function*() {

      const client = getEnv(self).api as ApiClientType;

      self.loading = true;
      try {
        const basket = new DTOs.GetBasket();
        const basketResponse: DTOs.QueryResponse<DTOs.Basket> = yield client.query(basket);

        self.basket = BasketModel.create(basketResponse.payload);

        const items = new DTOs.GetBasketItems();
        items.basketId = self.basket.id;
        const itemsResponse: DTOs.PagedResponse<DTOs.BasketItemIndex> = yield client.paged(items);

        self.items.clear();
        itemsResponse.records.forEach(item => {
          self.items.put(item);
        });

      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
      self.loading = false;
    });

    return { get };
  });
