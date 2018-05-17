import { types, flow, getEnv, getParent, applySnapshot, getSnapshot } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { ItemIndexType, ItemIndexModel } from '../../../models/basket/items';

const debug = new Debug('basket item quantity');

export interface QuantityFormType {
  id: string;
  quantity: number;
  readonly form: { [idx: string]: FieldDefinition };
  submit: () => Promise<{}>;
}
export const QuantityFormModel = types
  .model({
    productId: types.string,
    basketId: types.string,
    quantity: types.maybe(types.number)
  })
  .views(self => ({
    get validation() {
      const validation = {
        quantity: rules.quantity
      };

      return validate(self, validation);
    }
  }))
  .views(self => ({
    get form(): {[idx: string]: FieldDefinition} {
      return ({
        quantity: {
          input: 'number',
          label: 'Quantity',
          required: true,
        }
      });
    }
  }))
  .actions(self => {
    const submit = flow(function*() {
      const request = new DTOs.UpdateBasketItemQuantity();

      request.basketId = self.basketId;
      request.productId = self.productId;
      request.quantity = self.quantity;

      try {
        const client = getEnv(self).api as ApiClientType;
        const result: DTOs.CommandResponse = yield client.command(request);

      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
    });

    return { submit };
  });
