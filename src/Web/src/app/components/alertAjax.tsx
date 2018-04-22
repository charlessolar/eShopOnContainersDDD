import * as React from 'react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';

import Debug from 'debug';

import Alert from './alert';
import { Context } from '../context';
import { Response } from '../utils/store';

const debug = new Debug('alertAjax');

interface AlertAjaxProps {
  response: Response;
}

export default function AlertAjax(context: Context) {
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
