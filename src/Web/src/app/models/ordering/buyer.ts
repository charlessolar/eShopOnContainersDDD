import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

export interface BuyerType {
  id: string;
  givenName: string;

  goodStanding: boolean;

  preferredAddressId: string;
  preferredPaymentMethodId: string;
}
export const BuyerModel = types
  .model('Ordering_Buyer', {
    id: types.identifier(types.string),
    givenName: types.string,

    goodStanding: types.boolean,

    preferredAddressId: types.maybe(types.string),
    preferredPaymentMethodId: types.maybe(types.string)
  });

export interface BuyerIndexType {
  id: string;
  givenName: string;

  goodStanding: boolean;

  totalSpent: number;

  preferredCity?: string;
  preferredState?: string;
  preferredCountry?: string;
  preferredZipCode?: string;

  preferredPaymentCardholder?: string;
  preferredPaymentMethod?: string;
  preferredPaymentExpiration?: string;
}
export const BuyerIndexModel = types
  .model('Ordering_BuyerIndex', {
    id: types.identifier(types.string),
    givenName: types.string,

    goodStanding: types.boolean,

    totalSpent: types.number,

    preferredCity: types.maybe(types.string),
    preferredState: types.maybe(types.string),
    preferredCountry: types.maybe(types.string),
    preferredZipCode: types.maybe(types.string),

    preferredPaymentCardholder: types.maybe(types.string),
    preferredPaymentMethod: types.maybe(types.string),
    preferredPaymentExpiration: types.maybe(types.string),
  });
