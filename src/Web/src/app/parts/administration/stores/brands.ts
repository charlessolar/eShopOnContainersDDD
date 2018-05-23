import Debug from 'debug';
import { flow, getEnv, types } from 'mobx-state-tree';
import uuid from 'uuid/v4';
import * as validate from 'validate.js';
import { FieldDefinition } from '../../../components/models';
import { ApiClientType } from '../../../stores';
import { DTOs } from '../../../utils/eShop.dtos';
import rules from '../validation';

const debug = new Debug('catalog brands');

export interface BrandFormType {
  id: string;
  brand: string;
  readonly form: { [idx: string]: FieldDefinition };
  submit: () => Promise<{}>;
}
export const BrandFormModel = types
  .model({
    id: types.optional(types.identifier(types.string), uuid),
    brand: types.maybe(types.string)
  })
  .views(self => ({
    get validation() {
      const validation = {
        brand: rules.brand
      };

      return validate(self, validation);
    }
  }))
  .views(self => ({
    get form(): {[idx: string]: FieldDefinition} {
      return ({
        brand: {
          input: 'text',
          label: 'Brand',
          required: true,
        }
      });
    }
  }))
  .actions(self => {
    const submit = flow(function*() {
      const request = new DTOs.AddCatalogBrand();

      request.brandId = self.id;
      request.brand = self.brand;

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
