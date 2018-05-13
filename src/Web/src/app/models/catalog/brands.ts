import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

export interface BrandType {
  id: string;
  brand: string;
}
export const BrandModel = types
  .model('Catalog_Brand', {
    id: types.identifier(types.string),
    brand: types.string
  });
