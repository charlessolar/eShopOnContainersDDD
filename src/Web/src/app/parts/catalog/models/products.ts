import Debug from 'debug';
import { FormatDefinition } from '../../../components/models';
import { ProductModel as ProductModelBase, ProductType as ProductTypeBase } from '../../../models/catalog/products';

const debug = new Debug('catalog products');

export interface ProductType extends ProductTypeBase {
  readonly formatting?: {[idx: string]: FormatDefinition };
  readonly productPicture?: string;
  readonly canOrder?: boolean;
}
export const ProductModel =
  ProductModelBase
  .views(self => ({
    get formatting() {
      return ({
        price: {
          currency: true,
          normalize: 2
        }
      });
    },
    get productPicture() {
      if (!self.pictureContents) {
        return;
      }
      return 'data:' + self.pictureContentType + ';base64,' + self.pictureContents;
    },
    get canOrder() {
      return self.availableStock > 0 || self.onReorder;
    }
  }));
