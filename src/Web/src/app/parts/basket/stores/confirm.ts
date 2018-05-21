import { types, flow, getEnv } from 'mobx-state-tree';
import uuid from 'uuid/v4';
import Debug from 'debug';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { AddressType, AddressModel } from '../models/address';
import { PaymentMethodType, PaymentMethodModel } from '../models/paymentMethod';

const debug = new Debug('order confirm');

export interface ConfirmStoreType {
  id: string;
  basketId: string;
  billingAddress: AddressType;
  shippingAddress: AddressType;
  paymentMethod: PaymentMethodType;

  submit: () => Promise<{}>;
}
export const ConfirmStoreModel = types
  .model({
    id: types.optional(types.identifier(types.string), uuid),
    basketId: types.string,
    billingAddress: AddressModel,
    shippingAddress: AddressModel,
    paymentMethod: PaymentMethodModel
  })
  .actions(self => {
    const submit = flow(function*() {
      const request = new DTOs.DraftOrder();

      request.orderId = self.id;
      request.basketId = self.basketId;
      request.billingAddressId = self.billingAddress.id;
      request.shippingAddressId = self.shippingAddress.id;
      request.paymentMethodId = self.paymentMethod.id;

      try {
        const client = getEnv(self).api as ApiClientType;
        yield client.command(request);

       } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
    });

    return { submit };
  });
