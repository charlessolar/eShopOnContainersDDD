import Debug from 'debug';
import { flow, getEnv, types } from 'mobx-state-tree';
import { FormatDefinition } from '../../../components/models';
import { ProductModel as ProductModelBase, ProductType as ProductTypeBase } from '../../../models/catalog/products';
import { ApiClientType } from '../../../stores';
import { DTOs } from '../../../utils/eShop.dtos';

const debug = new Debug('catalog products');

export interface ProductType extends ProductTypeBase {
  updateStock: (stock: number) => void;
  readonly formatting: {[idx: string]: FormatDefinition };
  readonly productPicture: string;
  readonly shouldReorder: boolean;
}
export const ProductModel = ProductModelBase
.actions(self => ({
  updateStock(stock: number) {
    self.availableStock = stock;
  }
}))
.views(self => ({
  get formatting() {
    return ({
      price: {
        currency: true,
        normalize: 2
      },
      availableStock: {
        trim: true
      },
      restockThreshold: {
        trim: true
      },
      maxStockThreshold: {
        trim: true
      }
    });
  },
  get productPicture() {
    if (!self.pictureContents) {
      return;
    }
    return 'data:' + self.pictureContentType + ';base64,' + self.pictureContents;
  },
  get shouldReorder() {
    return self.restockThreshold > 0 && self.availableStock < self.restockThreshold;
  }
}));
