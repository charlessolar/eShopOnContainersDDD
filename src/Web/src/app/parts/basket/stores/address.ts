import { types, flow, getEnv, IStateTreeNode } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FormatDefinition, FieldDefinition } from '../../../components/models';

import { ApiClientType } from '../../../stores';

import { AddressType, AddressModel } from '../../../models/ordering/address';

import { AddressListType, AddressListModel } from '../models/address';
import AddressFormView from '../components/addressForm';

const debug = new Debug('buyer address');

export interface AddressStoreType {
  billingAddress: AddressType;
  shippingAddress: AddressType;

  readonly validation: {[idx: string]: FormatDefinition };
  readonly form: {[idx: string]: FieldDefinition };
  submit: () => Promise<{}>;
}
export const AddressStoreModel = types
  .model({
    billingAddress: types.maybe(types.union(types.string, AddressModel)),
    shippingAddress: types.maybe(types.union(types.string, AddressModel))
  })
  .views(self => ({
    get validation() {
      const validation = {
        billingAddress: rules.address,
        shippingAddress: rules.address
      };
      return validate(self, validation);
    },
    get form(): { [idx: string]: FieldDefinition } {
      return ({
        billingAddress: {
          input: 'selecter',
          label: 'Billing Address',
          required: true,
          selectStore: AddressListModel,
          addComponent: AddressFormView,
        },
        shippingAddress: {
          input: 'selecter',
          label: 'Shipping Address',
          required: true,
          selectStore: AddressListModel,
          addComponent: AddressFormView,
        }
      });
    }
  }))
  .actions(self => ({
    submit() {
      return new Promise(resolve => resolve());
    }
  }));
