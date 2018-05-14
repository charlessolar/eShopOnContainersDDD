
import { IType, types } from 'mobx-state-tree';
import { PagedResponse, QueryResponse } from './models';

export { inject, inject_props, inject_factory } from './inject';
export const models = {
  paged: PagedResponse,
  query: QueryResponse,
};

const ensureArray = (config: any | any[]) => config && (Array.isArray(config) ? config : [config]) || [];
export const when = (condition: any, config: any | any[], negativeConfig?: any | any[]) =>
  condition ? ensureArray(config) : ensureArray(negativeConfig);
