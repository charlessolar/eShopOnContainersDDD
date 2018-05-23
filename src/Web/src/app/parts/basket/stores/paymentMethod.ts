import Debug from 'debug';
import { types } from 'mobx-state-tree';
import * as validate from 'validate.js';
import { FieldDefinition, FormatDefinition } from '../../../components/models';
import { PaymentMethodModel, PaymentMethodType } from '../../../models/ordering/paymentMethod';
import PaymentMethodFormView from '../components/paymentMethodForm';
import { PaymentMethodListModel } from '../models/paymentMethod';
import rules from '../validation';

const debug = new Debug('buyer payment method');

export interface PaymentMethodStoreType {
  paymentMethod: PaymentMethodType;

  readonly validation: {[idx: string]: FormatDefinition };
  readonly form: {[idx: string]: FieldDefinition };
  submit: () => Promise<{}>;
}
export const PaymentMethodStoreModel = types
  .model({
    paymentMethod: types.maybe(types.union(types.string, PaymentMethodModel))
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
  }))
  .actions(self => ({
    submit() {
      return new Promise(resolve => resolve());
    }
  }));
