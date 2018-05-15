import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Button from 'material-ui/Button';

import { UsingContext, UsingType } from './using';

interface ActionProps {
  action: string;
  text: string;

  buttonProps?: any;
  className?: any;
  onSuccess?: () => void;
  onError?: () => void;
}

export class Action extends React.Component<ActionProps, {}> {

  public render() {

    return (
      <UsingContext.Consumer>
        {value => (
          <ActionConsumer using={value} {...this.props}/>
        )}
      </UsingContext.Consumer>
    );
  }
}

// need to wrap this in observer to make magic happen
const ActionConsumer = observer((props: ActionProps & { using: UsingType<any> }) => {
  const { using, text, action, buttonProps, className,  onSuccess, onError } = props;

  const handleAction = () => {
    try {
      using.action(action);
      if (onSuccess) {
        onSuccess();
      }
    } catch {
      if (onError) {
        onError();
      }
    }
  };

  return (
    <Button disabled={using.loading} onClick={handleAction} className={className} {...buttonProps}>{text}</Button>
  );
});
