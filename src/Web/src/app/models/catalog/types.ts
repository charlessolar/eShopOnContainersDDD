import { types } from 'mobx-state-tree';

export interface TypeType {
  id: string;
  type: string;
}
export const TypeModel = types
  .model('Catalog_Type', {
    id: types.identifier(types.string),
    type: types.string
  });
