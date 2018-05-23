import { types } from 'mobx-state-tree';

export interface OrderItemType {
  id: string;
  orderId: string;
  productId: string;

  productPictureContents: string;
  productPictureContentType: string;
  productName: string;
  productDescription: string;
  productPrice: number;
  price?: number;

  quantity: number;
  subTotal: number;
  additionalFees: number;
  additionalTaxes: number;
  total: number;
}
export const OrderItemModel = types
  .model('Ordering_Order_Item', {
    id: types.identifier(types.string),
    orderId: types.string,
    productId: types.string,

    productPictureContents: types.maybe(types.string),
    productPictureContentType: types.maybe(types.string),
    productName: types.string,
    productDescription: types.string,
    productPrice: types.number,
    price: types.maybe(types.number),

    quantity: types.number,
    subTotal: types.number,
    additionalFees: types.number,
    additionalTaxes: types.number,
    total: types.number,
  });
