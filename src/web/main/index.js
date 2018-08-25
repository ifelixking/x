import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter, Route, IndexRoute } from 'react-router-dom'
import { createStore, combineReducers, applyMiddleware } from 'redux'
import { Provider } from 'react-redux'
import thunk from 'redux-thunk'

import Nav from '../common/Nav'
import Resource from './resource'
import ActorPage from './actor'
import ActorDetail from './ActorDetail'
import InfoPage from './info'
import InfoPub from './InfoPub'
import InfoDetail from './InfoDetail'
import News from './news'

const App = () => (
  <BrowserRouter>
    <div>
      <header>
        <Nav items={[
          { text: '最新', href: '/' },
          { text: '资源', href: '/resource' },
          { text: '演员', href: '/actor' },
          { text: '信息', href: '/info' },
        ]} />
      </header>
      <div style={{ position: 'absolute', height: '100%', width:'100%', boxSizing: 'border-box', top: '0px', paddingTop: '85px' }}>
        <div style={{ height: '100%', overflow: 'auto' }}>
          <Route path="/" exact component={News} />
          <Route path="/resource" exact component={Resource.Component} />
          <Route path="/actor" exact children={({ match }) => (<ActorPage.Component visible={match && match.isExact} />)} />
          <Route path="/actor/:id" exact component={ActorDetail} />
          <Route path="/info" exact component={InfoPage.Component} />
          <Route path="/info/:id" exact component={InfoDetail} />
          <Route path="/infopub" exact component={InfoPub} />
        </div>
      </div>
    </div>
  </BrowserRouter>
)

let store = createStore(combineReducers({ home: Resource.Reducers, actor: ActorPage.Reducers, info: InfoPage.Reducers }), applyMiddleware(thunk))

ReactDOM.render((
  <Provider store={store}>
    <App />
  </Provider>
), document.getElementById('root'))