
import { IModelType } from 'mobx-state-tree';
import { inject as mobx_inject } from 'mobx-react';

import { StoreType } from '../stores';

export function inject<S, T>(storeType: IModelType<S, T>) {
  return mobx_inject((stores: { store: StoreType }, props) => {
    props['store'] = storeType.create(undefined, { api: stores.store.api });
    return props;
  });
}

export function inject_props<S, T>(accessor: (props: any) => IModelType<S, T>) {
  return mobx_inject((stores: { store: StoreType }, props) => {
    props['store'] = accessor(props).create(undefined, { api: stores.store.api });
    return props;
  });
}
