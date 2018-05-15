
import { IModelType, getSnapshot } from 'mobx-state-tree';
import { inject as mobx_inject } from 'mobx-react';

import { StoreType } from '../stores';

export function inject<S, T>(storeType: IModelType<S, T>, prop?: string, snapshotProp?: string, snapshotProjection?: (snapshot: any) => any) {
  if (!snapshotProjection) {
    snapshotProjection = (snapshot) => snapshot;
  }

  return mobx_inject((stores: { store: StoreType }, props) => {
    props[prop || 'store'] = storeType.create(snapshotProp && props[snapshotProp] ? snapshotProjection(getSnapshot(props[snapshotProp])) : {}, { api: stores.store.api });
    return props;
  });
}
export function inject_factory(factory: (store: StoreType) => any, prop?: string) {
  return mobx_inject((stores: { store: StoreType}, props) => {
    props[prop || 'store'] = factory(stores.store);
    return props;
  });
}

export function inject_props<S, T>(accessor: (props: any) => (store: StoreType) => any, prop?: string) {
  return mobx_inject((stores: { store: StoreType }, props) => {
    const storeType = accessor(props);
    if (!storeType) {
      return props;
    }
    props[prop || 'store'] = storeType(stores.store);
    return props;
  });
}
