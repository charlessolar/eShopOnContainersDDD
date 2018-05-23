import Debug from 'debug';
import { History } from 'history';
import { flow, getEnv, types } from 'mobx-state-tree';
import { FieldDefinition } from '../../../components/models';
import { ApiClientType } from '../../../stores';
import { DTOs } from '../../../utils/eShop.dtos';
import { BasketModel, BasketType } from '../models/basket';
import { BrandListModel, BrandModel, BrandType } from '../models/brands';
import { ProductModel, ProductType } from '../models/products';
import { TypeListModel, TypeModel, TypeType } from '../models/types';

const debug = new Debug('catalog');

export interface CatalogStoreType {
  products: Map<string, ProductType>;
  basket: BasketType;

  catalogType: TypeType;
  catalogBrand: BrandType;
  search: string;
  loading: boolean;

  basketItems: number;

  readonly validation: any;
  readonly form: {[idx: string]: FieldDefinition};
  get: () => Promise<{}>;
  addToBasket: (product: ProductType) => Promise<{}>;
  openBasket: () => void;
}
export const CatalogStoreModel = types
  .model('CatalogStore',
  {
    products: types.optional(types.map(ProductModel), {}),
    basket: types.optional(BasketModel, {}),

    catalogType: types.maybe(TypeModel),
    catalogBrand: types.maybe(BrandModel),

    search: types.optional(types.string, ''),

    loading: types.optional(types.boolean, false)
  })
  .views(self => ({
    get validation() {
      return;
    },
    get form(): { [idx: string]: FieldDefinition } {
      return ({
        search: {
          input: 'text',
          label: 'Search',
        },
        catalogType: {
          input: 'selecter',
          label: 'Catalog Type',
          selectStore: TypeListModel,
        },
        catalogBrand: {
          input: 'selecter',
          label: 'Catalog Brand',
          selectStore: BrandListModel,
        }
      });
    },
    get basketItems() {
      return self.basket.totalItems;
    }
  }))
  .actions(self => {
    const get = flow(function*() {

      const request = new DTOs.Catalog();

      if (self.catalogBrand) {
        request.brandId = self.catalogBrand.id;
      }
      if (self.catalogType) {
        request.typeId = self.catalogType.id;
      }
      if (self.search) {
        request.search = self.search;
      }

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;
        const result: DTOs.PagedResponse<DTOs.CatalogProductIndex> = yield client.paged(request);

        self.products.clear();
        result.records.forEach((record) => {
          self.products.put(record);
        });
       } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
      self.loading = false;
    });
    const addToBasket = flow(function*(product: ProductType) {
      yield self.basket.addToCart(product);
    });
    const openBasket = () => {
      const history = getEnv(self).history as History;
      history.push('/basket');
    };

    return { get, addToBasket, openBasket };
  });
