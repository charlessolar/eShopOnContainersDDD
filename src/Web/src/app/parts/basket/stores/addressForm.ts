import Debug from 'debug';
import { flow, getEnv, types } from 'mobx-state-tree';
import uuid from 'uuid/v4';
import * as validate from 'validate.js';
import { FieldDefinition } from '../../../components/models';
import { ApiClientType } from '../../../stores';
import { DTOs } from '../../../utils/eShop.dtos';
import rules from '../validation';

const debug = new Debug('buyer address');

export interface AddressFormType {
  id: string;
  alias: string;
  street: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  readonly form: { [idx: string]: FieldDefinition };
  submit: () => Promise<{}>;
}
export const AddressFormModel = types
  .model({
    id: types.optional(types.identifier(types.string), uuid),
    alias: types.maybe(types.string),
    street: types.maybe(types.string),
    city: types.maybe(types.string),
    state: types.maybe(types.string),
    zipCode: types.maybe(types.string),
    country: types.maybe(types.string)
  })
  .views(self => ({
    get validation() {
      const validation = {
        alias: rules.addressForm.alias,
        street: rules.addressForm.street,
        city: rules.addressForm.city,
        state: rules.addressForm.state,
        zipCode: rules.addressForm.zipCode,
        country: rules.addressForm.country
      };

      return validate(self, validation);
    }
  }))
  .views(self => ({
    get form(): {[idx: string]: FieldDefinition} {
      return ({
        alias: {
          input: 'text',
          label: 'Alias',
          required: true,
        },
        street: {
          input: 'text',
          label: 'Street',
          required: true,
        },
        city: {
          input: 'text',
          label: 'City',
          required: true,
        },
        state: {
          input: 'text',
          label: 'State',
          required: true,
        },
        zipCode: {
          input: 'text',
          label: 'Zip Code',
          required: true
        },
        country: {
          input: 'text',
          label: 'Country',
          required: true
        }
      });
    }
  }))
  .actions(self => {
    const submit = flow(function*() {
      const request = new DTOs.AddBuyerAddress();

      request.addressId = self.id;
      request.alias = self.alias;
      request.street = self.street;
      request.city = self.city;
      request.state = self.state;
      request.zipCode = self.zipCode;
      request.country = self.country;

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
