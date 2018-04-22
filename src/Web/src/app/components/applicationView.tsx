import * as React from 'react';
import { observer } from 'mobx-react';
import glamorous from 'glamorous';

import asyncView from './asyncView';
import { Context } from '../context';

import navbar from './navbar';
import footer from './footer';

interface AppViewProps {
  authenticated: boolean;
  email: string;
}

export default function ApplicationView(context: Context) {
  const { parts, theme } = context;
  const Footer = footer();
  const NavBar = navbar(context);

  const AppRoot = glamorous('div')({
    display: 'flex',
    minHeight: '100vh'
  });

  const AppView = glamorous('div')((_) => ({
    flex: '1 1 auto',
    display: 'flex',
    flexDirection: 'column',
    overflow: 'auto',
    color: theme.palette.primary.main
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
      const { authenticated, email, children } = this.props;

      return (
        <AppRoot>
          <AppView>
            <NavBar title={context.config.title} authenticated={authenticated} email={email}/>
            <MainView>
              {children}
            </MainView>
            <Footer title={context.config.title} version={context.config.build.version} />
          </AppView>
        </AppRoot>
      );

    }
  };
}
