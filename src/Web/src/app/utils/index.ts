
import { IType, types } from 'mobx-state-tree';
import { PagedResponse, QueryResponse } from './models';

export { inject, inject_props, inject_factory } from './inject';
export const models = {
  paged: PagedResponse,
  query: QueryResponse,
};
