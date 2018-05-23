import { types } from 'mobx-state-tree';

export interface ItemType {
  id: string;
  basketId: string;
  productId: string;

  productPictureContents?: string;
  productPictureContentType?: string;
  productName: string;
  productDescription?: string;

  productPrice: number;
  quantity: number;

  subTotal: number;
}

export const ItemModel = types
  .model('Basket_Basket_Item', {
    id: types.identifier(types.string),
    basketId: types.string,
    productId: types.string,

    productPictureContents: types.maybe(types.string),
    productPictureContentType: types.maybe(types.string),
    productName: types.string,
    productDescription: types.maybe(types.string),

    productPrice: types.number,
    quantity: types.number,

    subTotal: types.number,
  });

export interface ItemIndexType {
  id: string;
  basketId: string;
  productId: string;

  productPictureContents?: string;
  productPictureContentType?: string;
  productName: string;
  productDescription?: string;

  productPrice: number;
  quantity: number;

  subTotal: number;
}

export const ItemIndexModel = types
  .model('Basket_Basket_ItemIndex', {
    id: types.identifier(types.string),
    basketId: types.string,
    productId: types.string,

    productPictureContents: types.maybe(types.string),
    productPictureContentType: types.maybe(types.string),
    productName: types.string,
    productDescription: types.maybe(types.string),

    productPrice: types.number,
    quantity: types.number,

    subTotal: types.number
  });
