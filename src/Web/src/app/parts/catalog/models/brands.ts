import { types, getRoot, flow } from 'mobx-state-tree';
import { models } from '../../../utils';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClient } from '../../../stores';

export const Brand = types
  .model('Category.Brand', {
    id: types.identifier(types.string),
    brand: types.string,
    httpError: types.null
  })
  .views(self => ({
    get client(): ApiClient {
      return getRoot(this).api;
    },
    get success() {
      return self.httpError === null;
    }
  }))
  .actions(self => {
    const add = flow(function*() {
      const request = new DTOs.AddCategoryBrand();

      request.brandId = self.id;
      request.brand = self.brand;

      try {
        const result: DTOs.CommandResponse = yield self.client.command(request);

      } catch (error) {
        self.httpError = error;
      }
    });
    const remove = flow(function*() {
      const request = new DTOs.RemoveCategoryBrand();

      request.brandId = self.id;

      try {
        const result: DTOs.CommandResponse = yield self.client.command(request);
      } catch (error) {
        self.httpError = error;
      }
    });

    return { add, remove };
  });

export const List = types
  .model('Category.Brand.List', {
    response: types.maybe(models.paged(Brand)),
    loading: types.boolean,
    httpError: types.null
  })
  .views(self => ({
    get client(): ApiClient {
      return getRoot(this).api;
    },
    get success() {
      return self.httpError === null || self.response.responseStatus.success;
    }
  }))
  .actions(self => {
    const list = flow(function*(term: string, limit?: number) {
      const request = new DTOs.ListCategoryBrands();

      request.term = term;
      request.limit = limit || 10;

      self.loading = true;
      try {
        const results: DTOs.PagedResponse<DTOs.CategoryBrand> = yield self.client.paged(request);

        self.loading = false;
        self.response = models.paged(Brand).create(results);
      } catch (error) {
        self.httpError = error;
      }

    });

    return { list };
  });
