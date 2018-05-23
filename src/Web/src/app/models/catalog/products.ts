import { types } from 'mobx-state-tree';

export interface ProductType {
  id: string;
  name: string;
  description: string;
  price: number;
  catalogTypeId: string;
  catalogType: string;
  catalogBrandId: string;
  catalogBrand: string;

  availableStock: number;
  restockThreshold: number;
  maxStockThreshold: number;

  onReorder: boolean;

  pictureContents?: string;
  pictureContentType?: string;
}
export const ProductModel = types
  .model('Catalog_Product', {
    id: types.identifier(types.string),
    name: types.string,
    description: types.maybe(types.string),
    price: types.number,

    catalogTypeId: types.string,
    catalogType: types.string,
    catalogBrandId: types.string,
    catalogBrand: types.string,

    availableStock: types.number,
    restockThreshold: types.number,
    maxStockThreshold: types.number,

    onReorder: types.boolean,
    pictureContents: types.maybe(types.string),
    pictureContentType: types.maybe(types.string)
  });
