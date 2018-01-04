import * as React from 'react'

export default class FormInput extends React.Component<{ label: string, error: string }, any> {
  public render() {
    //console.log(this.props.airlines)
    return <div className={'form-group ' + (this.props.error ? 'has-error' : '')}>
      <label className="control-label">{this.props.label}</label>
      {this.props.children}
      {
        this.props.error &&
        <span className='control-label'>{this.props.error}</span>
      }
    </div>
  }
}