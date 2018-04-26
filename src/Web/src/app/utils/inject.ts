
import { inject as mobx_inject } from 'mobx-react';

import { StoreType } from '../stores';

export function inject(selector: (store: StoreType) => any) {
  return mobx_inject((stores: { store: StoreType }, props) => {
    props['store'] = selector(stores.store);
    return props;
  });
}
