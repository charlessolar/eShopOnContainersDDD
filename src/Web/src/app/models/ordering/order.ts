import { types } from 'mobx-state-tree';

import { OrderItemType, OrderItemModel } from './orderitem';

export interface OrderType {
  id: string;
  status: string;
  statusDescription: string;

  userName: string;
  buyerName: string;

  shippingAddressId: string;
  shippingAddress: string;
  shippingCityState: string;
  shippingZipCode: string;
  shippingCountry: string;
  billingAddressId: string;
  billingAddress: string;
  billingCityState: string;
  billingZipCode: string;
  billingCountry: string;

  paymentMethodId: string;
  paymentMethod: string;

  totalItems: number;
  totalQuantity: number;

  subTotal: number;

  additionalFees: number;
  additionalTaxes: number;

  total: number;

  created: number;
  updated: number;

  paid: boolean;

  items: OrderItemType[];
}
export const OrderModel = types
  .model('Ordering_Order', {
    id: types.identifier(types.string),
    status: types.string,
    statusDescription: types.string,

    userName: types.string,
    buyerName: types.string,

    shippingAddressId: types.string,
    shippingAddress: types.string,
    shippingCityState: types.string,
    shippingZipCode: types.string,
    shippingCountry: types.string,
    billingAddressId: types.string,
    billingAddress: types.string,
    billingCityState: types.string,
    billingZipCode: types.string,
    billingCountry: types.string,

    paymentMethodId: types.string,
    paymentMethod: types.string,
    totalItems: types.number,
    totalQuantity: types.number,

    subTotal: types.number,
    additionalFees: types.number,
    additionalTaxes: types.number,
    total: types.number,

    created: types.number,
    updated: types.number,

    paid: types.boolean,

    items: types.array(OrderItemModel)
  });

export interface OrderIndexType {
  id: string;
  status: string;
  statusDescription: string;

  userName: string;
  buyerName: string;

  shippingAddressId: string;
  shippingAddress: string;
  shippingCity: string;
  shippingState: string;
  shippingZipCode: string;
  shippingCountry: string;
  billingAddressId: string;
  billingAddress: string;
  billingCity: string;
  billingState: string;
  billingZipCode: string;
  billingCountry: string;

  paymentMethodId: string;
  paymentMethod: string;

  totalItems: number;
  totalQuantity: number;

  subTotal: number;

  additional: number;

  total: number;

  created: number;
  updated: number;

  paid: boolean;
}
export const OrderIndexModel = types
  .model('Ordering_OrderIndex', {
    id: types.identifier(types.string),
    status: types.string,
    statusDescription: types.string,

    userName: types.string,
    buyerName: types.string,

    shippingAddressId: types.string,
    shippingAddress: types.string,
    shippingCity: types.string,
    shippingState: types.string,
    shippingZipCode: types.string,
    shippingCountry: types.string,
    billingAddressId: types.string,
    billingAddress: types.string,
    billingCity: types.string,
    billingState: types.string,
    billingZipCode: types.string,
    billingCountry: types.string,

    paymentMethodId: types.string,
    paymentMethod: types.string,
    totalItems: types.number,
    totalQuantity: types.number,

    subTotal: types.number,
    additional: types.number,
    total: types.number,

    created: types.number,
    updated: types.number,

    paid: types.boolean
  });
