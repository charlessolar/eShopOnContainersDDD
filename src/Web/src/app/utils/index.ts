
import { IType, types } from 'mobx-state-tree';
import { PagedResponse, QueryResponse, Form } from './models';

export { FormType, ComponentDefinition } from './models';

export { inject } from './inject';
export const models = {
  paged: PagedResponse,
  query: QueryResponse,
  form: Form,
};
