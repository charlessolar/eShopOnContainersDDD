import { types } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FormatDefinition, FieldDefinition } from '../../../components/models';

import { ApiClientType } from '../../../stores';

import { PaymentMethodType, PaymentMethodModel } from '../../../models/ordering/paymentMethod';

import { PaymentMethodListType, PaymentMethodListModel } from '../models/paymentMethod';
import PaymentMethodFormView from '../components/paymentMethodForm';

const debug = new Debug('buyer payment method');

export interface PaymentMethodStoreType {
  paymentMethod: PaymentMethodType;

  readonly validation: {[idx: string]: FormatDefinition };
  readonly form: {[idx: string]: FieldDefinition };
}
export const PaymentMethodStoreModel = types
  .model({
    paymentMethod: types.maybe(PaymentMethodModel)
  })
  .views(self => ({
    get validation() {
      const validation = {
        paymentMethod: rules.paymentMethod
      };
      return validate(self, validation);
    },
    get form(): { [idx: string]: FieldDefinition } {
      return ({
        paymentMethod: {
          input: 'selecter',
          label: 'Payment Method',
          required: true,
          selectStore: PaymentMethodListModel,
          addComponent: PaymentMethodFormView,
        }
      });
    }
  }));
