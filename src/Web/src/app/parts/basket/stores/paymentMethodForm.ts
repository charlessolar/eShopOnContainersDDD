import { types, flow, getEnv, getParent, applySnapshot, getSnapshot } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

const debug = new Debug('buyer payment method');

export interface PaymentMethodFormType {
  id: string;
  alias: string;
  cardNumber: string;
  securityNumber: string;
  cardholderName: string;
  expiration: string;
  cardType: string;

  readonly form: { [idx: string]: FieldDefinition };
  submit: () => Promise<{}>;
}
export const PaymentMethodFormModel = types
  .model({
    id: types.optional(types.identifier(types.string), uuid),
    alias: types.maybe(types.string),
    cardNumber: types.maybe(types.string),
    securityNumber: types.maybe(types.string),
    cardholderName: types.maybe(types.string),
    expiration: types.maybe(types.string),
    cardType: types.maybe(types.string)
  })
  .views(self => ({
    get validation() {
      const validation = {
        alias: rules.paymentMethodForm.alias,
        cardNumber: rules.paymentMethodForm.cardNumber,
        securityNumber: rules.paymentMethodForm.securityNumber,
        cardholderName: rules.paymentMethodForm.cardholderName,
        expiration: rules.paymentMethodForm.expiration,
        cardType: rules.paymentMethodForm.cardType
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
        cardNumber: {
          input: 'text',
          label: 'Card Number',
          required: true,
        },
        securityNumber: {
          input: 'text',
          label: 'Security Number',
          required: true,
        },
        cardholderName: {
          input: 'text',
          label: 'Cardholder Name',
          required: true,
        },
        expiration: {
          input: 'text',
          label: 'Expiration',
          required: true
        },
        cardType: {
          input: 'dropdown',
          label: 'Card Type',
          required: true,
          options: [
            { value: 'VISA', label: 'Visa' },
            { value: 'AMEX', label: 'Amex' },
            { value: 'MASTERCARD', label: 'Mastercard' },
          ]
        }
      });
    }
  }))
  .actions(self => {
    const submit = flow(function*() {
      const request = new DTOs.AddBuyerPaymentMethod();

      request.paymentMethodId = self.id;
      request.alias = self.alias;
      request.cardNumber = self.cardNumber;
      request.securityNumber = self.securityNumber;
      request.cardholderName = self.cardholderName;
      request.expiration = self.expiration;
      request.cardType = self.cardType;

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
