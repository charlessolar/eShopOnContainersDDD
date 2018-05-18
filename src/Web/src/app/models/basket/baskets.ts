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
  created: number;
  updated: number;
}
export const BasketModel = types
  .model('Basket_Basket', {
    id: types.identifier(types.string),
    customerId: types.optional(types.string, ''),
    customer:  types.optional(types.string, ''),
    totalItems: types.number,
    totalQuantity: types.number,
    subTotal: types.number,
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
  created: number;
  updated: number;
}
export const BasketIndexModel = types
  .model('Basket_BasketIndex', {
    id: types.identifier(types.string),
    customerId:  types.optional(types.string, ''),
    customer: types.optional(types.string, ''),
    totalItems: types.number,
    totalQuantity: types.number,
    subTotal: types.number,
    created: types.number,
    updated: types.number
  });
