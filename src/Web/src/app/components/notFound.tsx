
import * as React from 'react';

import { Context } from '../context';

export default function NotFound(context: Context) {
  return class extends React.Component<{}, {}> {
    public render() {
      return (<h2>NotFound</h2>);
    }
  };
}
