
import { IType, types } from 'mobx-state-tree';
import { PagedResponse, QueryResponse, Select } from './models';

export { SelectType } from './models';

export { inject, inject_props } from './inject';
export const models = {
  paged: PagedResponse,
  query: QueryResponse,
  select: Select
};
