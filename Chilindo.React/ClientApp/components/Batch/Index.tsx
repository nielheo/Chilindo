import * as React from 'react';
import { RouteComponentProps } from 'react-router';

import { Grid, Row, Col, Button, Table, Panel } from 'react-bootstrap'

import * as moment from 'moment'
import FormTextbox from '../Common/FormTextbox'
import FormDropdown from '../Common/FormDropdown'

import Submit from '../Common/Submit'
import Batch from './Batch'

let minBatch = 1, maxBatch = 10
let minTrans = 1, maxTrans = 20
let minInterval = 100, maxInterval = 5000

let results: any[] = []

export default class Home extends React.Component<RouteComponentProps<{}>, any> {
  constructor(props: any) {
    super(props)
    this.state = {
      accountNumber: 1234,
      batch: 5,
      transactionPerBatch: 10,
      interval: 1000,
      results: [],
    }
  }

  _onAccountNumberChange = (e: any) => {
    if (this.state.accountNumber !== e.target.value) {
      this.setState({ accountNumber: e.target.value })
    }
  }

  _onBatchChange = (e: any) => {
    let value = parseInt(e.target.value) || 0
    if (this.state.batch !== value) {
      this.setState({ batch: value })
    }
  }

  _onTrasactionPerBatchChange = (e: any) => {
    let value = parseInt(e.target.value) || 0
    if (this.state.transactionPerBatch !== value) {
      this.setState({ transactionPerBatch: value })
    }
  }

  _onIntervalChange = (e: any) => {
    let value = parseInt(e.target.value) || 0
    if (this.state.interval !== value) {
      this.setState({ interval: value })
    }
  }
  
  _validInput = () => {
    if (!this.state.accountNumber)
      return false

    if (this.state.batch < minBatch || this.state.batch > maxBatch)
      return false 

    if (this.state.transactionPerBatch < minTrans || this.state.transactionPerBatch > maxTrans)
      return false

    if (this.state.interval < minInterval || this.state.interval > maxInterval)
      return false

    return true
  }

  _addBatch = (index: number) => {
    results.push({
      index: index,
      started: moment()
    })
    this.setState({ results: results })

    Submit('balance', this.state.accountNumber).then((res: any) => {
      let result = results.filter((i: any) => i.index === index)[0]
      if (result)
        result.balance = res

      this.setState({ results: results })
    })
    
  }
  
  _submitRequest = () => {
    for (var i = 1; i <= this.state.batch; i++) {
      var tick = function (that: any, i:any) {
        return function () {
          that._addBatch(i)
        }
      };
      setTimeout(tick(this, i), this.state.interval * (i - 1));
    }
  }

  _onButtonClicked = () => {
    if (this._validInput()) {
      results = []
      this._submitRequest()
    } else {
      if (!this.state.clicked)
        this.setState({ clicked: true })
    }
  }

  _actionText = (a: string) => {
    switch (a.toLocaleLowerCase()) {
      case 'balance':
        return 'Check Balance'
      case 'deposit':
        return 'Deposit'
      case 'withdraw':
        return 'Withdraw'
      default:
        return ''
    }
  }

  public render() {
    //console.log(this.state.result)
    let idx = 1
    return <section>
      <Row>
        <Col md={12}><h2>Batch (Random) Transaction</h2></Col>
      </Row>
      <hr />
      <Row>
        <Col md={5}>
          <Row>
            <Col md={8}>
              <FormTextbox
                label={'Account Number'}
                disabled={false}
                value={this.state.accountNumber}
                onChange={this._onAccountNumberChange}
                error={this.state.clicked && !this.state.accountNumber ? '* required' : ''}
              />
            </Col>
          </Row>
          <Row>
            <Col md={8}>
              <FormTextbox
                label={'Number of Batch'}
                disabled={false}
                value={this.state.batch}
                onChange={this._onBatchChange}
                error={!(this.state.batch >= minBatch && this.state.batch <= maxBatch)
                  ? '* between ' + minBatch + ' - ' + maxBatch : ''}
              />
            </Col>
          </Row>
          <Row>
            <Col md={8}>
              <FormTextbox
                label={'Transaction / Batch'}
                disabled={false}
                value={this.state.transactionPerBatch}
                onChange={this._onTrasactionPerBatchChange}
                error={!(this.state.transactionPerBatch >= minTrans && this.state.transactionPerBatch <= maxTrans)
                  ? '* between ' + minTrans + ' - ' + maxTrans : ''}
              />
            </Col>
          </Row>
          <Row>
            <Col md={8}>
              <FormTextbox
                label={'Interval (milliseconds)'}
                disabled={false}
                value={this.state.interval}
                onChange={this._onIntervalChange}
                error={!(this.state.interval >= minInterval && this.state.interval <= maxInterval)
                  ? '* between ' + minInterval + ' - ' + maxInterval : ''}
              />
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <Button bsStyle='primary' onClick={this._onButtonClicked}>Submit</Button>
            </Col>
          </Row>
        </Col>
        <Col md={7}>
          {
            this.state.results.map((res: any) => <Batch result={res} key={res.index} /> )
          }
        </Col>
      </Row>
    </section>
  }
}
