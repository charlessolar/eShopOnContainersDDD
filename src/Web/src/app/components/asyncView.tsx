import * as React from 'react';
import { observable, action, runInAction } from 'mobx';
import { observer } from 'mobx-react';
import { IModelType, destroy } from 'mobx-state-tree';

import Fade from 'material-ui/transitions/Fade';
import { CircularProgress } from 'material-ui/Progress';

import { inject_props } from '../utils';
import { StoreType } from '../stores';

class AsyncStore {

  @observable
  public loading: boolean;
  @observable
  public error: boolean;
  @observable
  public componentData: any;

  private waitingFor: number;

  @action
  public loadComponent(store: any, promiseAction?: (store: any) => Promise<{}>, component?: any, getComponent?: (check?: number, cb?: (props: AsyncViewProps) => void) => Promise<any>) {

    if (promiseAction) {
      this.loading = true;
      promiseAction(store).then(action(() => {
        this.loading = false;
      }));
    }

    if (component) {
      this.componentData = component;
      return;
    }

    this.waitingFor = Math.random();

    const componentData = getComponent(
      this.waitingFor,
      ({ component }) => {
        // Named param for making callback future proof
        runInAction(() => {
          if (component) {
            this.componentData = component;
          }
        });
      }
    );

    // In case returned value was a promise
    if (componentData && componentData.then) {

      // IIFE to check if a later ending promise was creating a race condition
      // Check test case for more info
      (check => {
        componentData.then((component: any) => {
          // Checks that the promise last invoked is the one we load
          if (this.waitingFor !== check) {
            return;
          }
          runInAction(() => {
            // component is `import`
            this.componentData = component.default;
          });
        }).catch(action((e: any) => {
          this.error = e;
        }));
      })(this.waitingFor);
    }
  }
}

interface AsyncViewProps {
  url?: string;
  component?: any;
  store?: any;

  existingStore?: any;
  actionStore?: (store: StoreType) => any;
  action?: (store: any) => Promise<{}>;

  getComponent?: (check?: number, cb?: (props: AsyncViewProps) => void) => Promise<any>;
  loading?: (store: any) => boolean;

  componentProps?: any;
}
interface AsyncViewState {
  componentData: any;
  error: any;
  loading: boolean;
}

@inject_props((props) => props.actionStore)
@observer
export default class AsyncView extends React.Component<AsyncViewProps, AsyncViewState> {
  private _asyncStore: AsyncStore;

  constructor(props: AsyncViewProps) {
    super(props);
    this._asyncStore = new AsyncStore();
  }

  public componentWillMount() {
    const { store, existingStore } = this.props;

    this._asyncStore.loadComponent(store || existingStore, this.props.action, this.props.component, this.props.getComponent);
  }
  public componentWillUnmount() {
    const { store } = this.props;

    if (store) {
      // don't destroy the store as any active promise will throw exceptions
      // https://github.com/mobxjs/mobx-state-tree/issues/792
      // destroy(store);
    }
  }

  public render() {
    const { store, existingStore } = this.props;

    if (this._asyncStore.componentData && !this._asyncStore.loading && (!this.props.loading || !(this.props.loading(store || existingStore)))) {
      return React.createElement(this._asyncStore.componentData, { store: store || existingStore, ...this.props, ...this.props.componentProps});
    }
    return (
      <div style={{
        height: '100%',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center'
      }}>
        <Fade
          in={true}
          unmountOnExit
        >
          <CircularProgress />
        </Fade>
      </div>
    );
  }
}
