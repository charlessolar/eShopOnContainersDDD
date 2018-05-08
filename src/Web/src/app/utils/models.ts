import { IModelType, types, flow, getSnapshot } from 'mobx-state-tree';
import { Component } from 'react';
import { StoreType } from '../stores';

const ResponseError = types.model(
  'ResponseError',
  {
    errorCode: types.optional(types.string, ''),
    fieldName: types.optional(types.string, ''),
    message: types.optional(types.string, ''),
    meta: types.optional(types.map(types.string), {})
  }
);

const ResponseStatus = types.model(
  'ResponseStatus',
  {
    errorCode: types.optional(types.string, ''),
    message: types.optional(types.string, ''),
    stackTrace: types.optional(types.string, ''),
    errors: types.optional(types.array(ResponseError), [])
  })
  .views(self => ({
    get success() {
      return self.errors.length === 0;
    }
  }));

export function PagedResponse<S, T>(subtype: IModelType<S, T>) {
  return types.model({
    roundTripMs: types.number,
    total: types.number,
    records: types.array(subtype),
    responseStatus: types.optional(ResponseStatus, {})
  });
}

export function QueryResponse<S, T>(subtype: IModelType<S, T>) {
  return types.model({
    roundTripMs: types.number,
    payload: subtype
  });
}

export interface SelectType {
  loading: boolean;
  term: string;
  records: { id: string, label: string }[];
  list: () => Promise<{}>;
  updateTerm: (newVal: string) => Promise<{}>;
}
export function Select<S, T>(rootStore: StoreType, projectionStore: any, projection: (store: any, term: string) => Promise<{ id: string, label: string}[]>) {

  return types.model(
    {
      loading: types.optional(types.boolean, false),
      term: types.optional(types.string, ''),
      records: types.optional(types.array(types.model({
        id: types.string,
        label: types.string
      })), [])
    })
    .actions(self => {
      const list = flow(function*() {
        self.loading = true;
        try {
          self.records = yield projection(projectionStore, self.term);
        } catch (e) {
          // console.log('error fetching:', e);
        }
        self.loading = false;
      });
      const updateTerm = flow(function*(newVal: string) {
        self.term = newVal;
        yield list();
      });
      return { list, updateTerm };
    })
    .create({}, { api: rootStore.api });
}
