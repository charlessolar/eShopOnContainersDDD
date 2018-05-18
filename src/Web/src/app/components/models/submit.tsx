import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';

import { UsingContext, UsingType } from './using';

interface SubmitProps {
  text?: string;
  buttonProps?: any;
  className?: any;
  onSuccess?: () => void;
  onError?: () => void;
}

export class Submit extends React.Component<SubmitProps, {}> {

  public render() {

    return (
      <UsingContext.Consumer>
        {value => (
          <SubmitConsumer using={value} {...this.props}/>
        )}
      </UsingContext.Consumer>
    );
  }
}

// need to wrap this in observer to make magic happen
const SubmitConsumer = observer((props: SubmitProps & { using: UsingType<any> }) => {
  const { using, text, buttonProps, className,  onSuccess, onError } = props;

  const handleSubmit = () => {
    try {
      using.submit();
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
    <Button disabled={!using.valid || using.loading} onClick={handleSubmit} className={className} {...buttonProps}>{text || 'Save'}</Button>
  );
});
