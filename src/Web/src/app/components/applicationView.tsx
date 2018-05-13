import * as React from 'react';
import { inject, observer } from 'mobx-react';
import glamorous from 'glamorous';

import asyncView from './asyncView';

import NavBar from './navbar';
import Footer from './footer';

import { StoreType } from '../stores';

interface AppViewProps {
  title: string;
  version: string;

  store?: StoreType;
}

const AppRoot = glamorous('div')({
  display: 'flex',
  minHeight: '100vh'
});

const AppView = glamorous('div')((_) => ({
  flex: '1 1 auto',
  display: 'flex',
  flexDirection: 'column',
  overflow: 'auto'
}));

const MainView = glamorous('main')({
  'display': 'flex',
  'flex': '1',
  'justifyContent': 'center',
  'alignItems': 'flex-start',
  '@media(max-width: 600px)': {
    margin: 10
  }
});

@inject('store')
@observer
export default class extends React.Component<AppViewProps, {}> {

  public render() {
    const { store, children, title, version } = this.props;

    return (
      <AppRoot>
        <AppView>
          <NavBar title={title} authenticated={store.authenticated} name={store.auth.name} />
          <MainView>
            {children}
          </MainView>
          <Footer title={title} version={version} />
        </AppView>
      </AppRoot>
    );

  }
}
