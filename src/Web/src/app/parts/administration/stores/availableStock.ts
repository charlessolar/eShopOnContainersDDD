import { types, flow, getEnv, getParent, applySnapshot, getSnapshot } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { ProductType, ProductModel } from '../../../models/catalog/products';

const debug = new Debug('product stock');

export interface StockFormType {
  id: string;
  availableStock: number;
  readonly form: { [idx: string]: FieldDefinition };
  submit: () => Promise<{}>;
}
export const StockFormModel = types
  .model({
    id: types.identifier(types.string),
    availableStock: types.maybe(types.number)
  })
  .views(self => ({
    get validation() {
      const validation = {
        availableStock: rules.stock
      };

      return validate(self, validation);
    }
  }))
  .views(self => ({
    get form(): {[idx: string]: FieldDefinition} {
      return ({
        availableStock: {
          input: 'number',
          label: 'Stock',
          required: true,
        }
      });
    }
  }))
  .actions(self => {
    const submit = flow(function*() {
      const request = new DTOs.UpdateStock();

      request.productId = self.id;
      request.stock = self.availableStock;

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
