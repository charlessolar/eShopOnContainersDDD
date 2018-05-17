import { types, getRoot, getEnv, flow, applyAction, applySnapshot, onSnapshot, addDisposer } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { BasketType, BasketModel } from '../models/basket';
import { ItemIndexType, ItemIndexModel } from '../models/items';

const debug = new Debug('basket');

export interface BasketStoreType {
  loading: boolean;
  basket: BasketType;
  items: Map<string, ItemIndexType>;

  readonly validation: any;
  readonly form: {[idx: string]: FieldDefinition};
  get: () => Promise<{}>;
  removeItem: (item: ItemIndexType) => void;
}

export const BasketStoreModel = types
  .model('BasketStore',
    {
      basketId: types.maybe(types.string),
      totalItems: types.optional(types.number, 0),
      loading: types.optional(types.boolean, false),

      basket: types.maybe(BasketModel),
      items: types.optional(types.map(ItemIndexModel), {}),

    })
    .views(self => ({
      get validation() {
        return;
      },
      get form(): { [idx: string]: FieldDefinition } {
        return ({
        });
      }
    }))
  .actions(self => {
    const get = flow(function*() {
      debug('getting basket items');

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
    const removeItem = flow(function*(item: ItemIndexType) {
      debug('removing a basket item');

      const client = getEnv(self).api as ApiClientType;
      try {
        const request = new DTOs.RemoveBasketItem();
        request.basketId = self.basketId;
        request.productId = item.productId;
        yield client.command(request);

        self.basket.subTotal -= item.subTotal;
        self.items.delete(item.id);
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
    };

    return { get, removeItem, afterCreate };
  });
