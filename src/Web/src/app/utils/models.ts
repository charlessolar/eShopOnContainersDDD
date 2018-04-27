import { IModelType, types, flow } from 'mobx-state-tree';
import { Component } from 'react';
import { StoreType } from '../stores';

export interface ComponentDefinition {
  input: 'text' | 'select' | 'textarea' | 'number';
  label: string;
  required?: boolean;

  projectionStore?: IModelType<{}, {}>;
  projection?: (store: any, term: string) => Promise<{ id: string, label: string}[]>;
}

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

export interface FormType<T> {
  loading: boolean;
  readonly valid: boolean;
  readonly form: { [idx: string]: ComponentDefinition };
  readonly validation: { [idx: string]: string };
  changeValue(name: string, newVal: any): void;
  submit(): void;
  payload: T;
}
export function Form<S, T extends { readonly form: { [idx: string]: ComponentDefinition }, readonly valid: boolean, readonly validation: { [idx: string]: string }, submit(): void }>(subtype: IModelType<S, T>, env: any, sideEffect?: (model: T, success: boolean) => void): FormType<T> {
  return types.model(
    {
      loading: types.optional(types.boolean, false),
      payload: types.optional(subtype, {})
    })
    .views(self => ({
      get valid() {
        return self.payload.valid;
      },
      get form() {
        return self.payload.form;
      },
      get validation() {
        return self.payload.validation;
      }
    }))
    .actions(self => {
      const changeValue = (name: string, newVal: any) => {
        self.payload[name] = newVal;
      };
      const submit = flow(function*() {
        self.loading = true;
        let success = false;
        try {
          yield self.payload.submit();
          success = true;
        } catch (e) {
          console.log('submit error:', e);
        }
        if (sideEffect) {
          sideEffect(self.payload, success);
        }
        self.loading = false;
      });
      return { changeValue, submit };
    })
    .create({}, env);
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
          console.log('error fetching:', e);
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
