import { types } from 'mobx-state-tree';

export interface BrandType {
  id: string;
  brand: string;
}
export const BrandModel = types
  .model('Catalog_Brand', {
    id: types.identifier(types.string),
    brand: types.string
  });
