import * as React from 'react'
import { Route } from 'react-router-dom'
import { Layout } from './components/Layout'
import { Home } from './components/Home'
import Batch from './components/Batch'


export const routes = <Layout>
  <Route exact path='/' component={Home} />
  <Route exact path='/batch' component={Batch} />

</Layout>;
