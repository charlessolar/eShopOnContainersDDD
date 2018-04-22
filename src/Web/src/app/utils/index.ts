export { inject } from './ioc';
export { Client } from './client';

import { IType, types } from 'mobx-state-tree';
import { PagedResponse, QueryResponse } from './models';

export const models = {
  paged: PagedResponse,
  query: QueryResponse
};
