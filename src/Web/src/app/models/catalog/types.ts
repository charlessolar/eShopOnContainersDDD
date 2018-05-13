import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

export interface TypeType {
  id: string;
  type: string;
}
export const TypeModel = types
  .model('Catalog_Type', {
    id: types.identifier(types.string),
    type: types.string
  });
