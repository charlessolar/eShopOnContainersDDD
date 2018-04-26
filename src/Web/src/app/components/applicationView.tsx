import * as React from 'react';
import { observer } from 'mobx-react';
import glamorous from 'glamorous';

import asyncView from './asyncView';

import navbar from './navbar';
import Footer from './footer';

interface AppViewProps {
  authenticated: boolean;
  email: string;
  title: string;
  version: string;
}

export default function ApplicationView() {
  const NavBar = navbar();

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

  return class extends React.Component<AppViewProps, {}> {

    public render() {
      const { authenticated, email, children, title, version } = this.props;

      return (
        <AppRoot>
          <AppView>
            <NavBar title={title} authenticated={authenticated} email={email}/>
            <MainView>
              {children}
            </MainView>
            <Footer title={title} version={version} />
          </AppView>
        </AppRoot>
      );

    }
  };
}
