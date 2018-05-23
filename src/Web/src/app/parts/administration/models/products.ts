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

export interface ProductListType {
  entries: Map<string, ProductType>;
  loading: boolean;
  list: () => Promise<{}>;
  add: (product: ProductType) => void;
  remove: (id: string) => Promise<{}>;
}

export const ProductListModel = types
  .model('Catalog_Product_List', {
    entries: types.optional(types.map(ProductModel), {}),
    loading: types.optional(types.boolean, true)
  })
  .actions(self => {
    const list = flow(function*() {
      const request = new DTOs.ListProducts();

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;
        const results: DTOs.PagedResponse<DTOs.CatalogProductIndex> = yield client.paged(request);

        results.records.forEach(record => {
          self.entries.put(record);
        });
      } catch (error) {
        debug('received http error: ', error);
      }
      self.loading = false;
    });
    const add = (product: ProductType) => {
      self.entries.put(product);
    };
    const remove = flow(function*(id: string) {
      const request = new DTOs.RemoveProduct();

      request.productId = id;

      try {
        const client = getEnv(self).api as ApiClientType;
        const result: DTOs.CommandResponse = yield client.command(request);

        self.entries.delete(id);
      } catch (error) {
        debug('received http error: ', error);
      }
    });

    return { list, add, remove };
  });
