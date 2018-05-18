import * as React from 'react';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';

import Debug from 'debug';

import Alert from './alert';

const debug = new Debug('alertAjax');

interface AlertAjaxProps {
  response: any;
}

export default function AlertAjax() {
  const styles = (theme: Theme) => ({});

  return withStyles(styles)(class extends React.Component<AlertAjaxProps & WithStyles<never>, {}> {

    public render() {
      const { response } = this.props;

      if (!response) {
        return null;
      }
      debug('error:', response);
      const { status } = response;

      if (![401, 422].includes(status)) {
        return null;
      }

      let message = response.data.message;
      if (!message) {
        message = 'Failed to process request';
      }

      return (<Alert type='error' message={message}/>);
    }
  });
}
