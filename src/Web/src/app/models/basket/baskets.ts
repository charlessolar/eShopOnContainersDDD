import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

export interface BasketType {
  id: string;
  customerId: string;
  customer: string;
  totalItems: number;
  totalQuantity: number;
  subTotal: number;
  totalFees: number;
  totalTaxes: number;
  total: number;
  created: number;
  updated: number;
}
export const BasketModel = types
  .model('Basket_Basket', {
    id: types.identifier(types.string),
    customerId: types.string,
    customer: types.string,
    totalItems: types.number,
    totalQuantity: types.number,
    subTotal: types.number,
    totalFees: types.number,
    totalTaxes: types.number,
    total: types.number,
    created: types.number,
    updated: types.number
  });

export interface BasketIndexType {
  id: string;
  customerId: string;
  customer: string;
  totalItems: number;
  totalQuantity: number;
  subTotal: number;
  extraTotal: number;
  total: number;
  created: number;
  updated: number;
}
export const BasketIndexModel = types
  .model('Basket_BasketIndex', {
    id: types.identifier(types.string),
    customerId: types.string,
    customer: types.string,
    totalItems: types.number,
    totalQuantity: types.number,
    subTotal: types.number,
    extraTotal: types.number,
    total: types.number,
    created: types.number,
    updated: types.number
  });
