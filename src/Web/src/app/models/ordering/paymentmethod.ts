import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

export interface PaymentMethodType {
  id: string;
  userName: string;

  alias: string;
  cardNumber: string;
  securityNumber: string;
  cardholderName: string;
  expiration: string;
  cardType: string;
}
export const PaymentMethodModel = types
  .model('Ordering_Buyer_PaymentMethod', {
    id: types.identifier(types.string),
    userName: types.string,

    alias: types.string,
    cardNumber: types.string,
    securityNumber: types.string,
    cardholderName: types.string,
    expiration: types.string,
    cardType: types.string
  });
