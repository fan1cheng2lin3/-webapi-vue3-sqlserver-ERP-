<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import * as echarts from 'echarts';

// 数据存储
const storehouseData = ref([]);

// 获取后端数据
const fetchStorehouseData = async () => {
  try {
    const response = await axios.get('/File_Management/storehouse');
    storehouseData.value = response.data;
    renderCharts();
  } catch (error) {
    console.error("获取仓库数据失败", error);
  }
};

// 绘制所有图表
const renderCharts = () => {
  // 1. 库存量统计图
  const inventoryChart = echarts.init(document.getElementById('inventory-chart'));
  const inventoryOption = {
    title: {
      text: '每个仓库的库存量',
      subtext: '单位：件',
    },
    tooltip: {
      trigger: 'axis',
    },
    xAxis: {
      type: 'category',
      data: getWarehouseNames(),
    },
    yAxis: {
      type: 'value',
    },
    series: [
      {
        name: '库存量',
        type: 'bar',
        data: getWarehouseStockCounts(),
      },
    ],
  };
  inventoryChart.setOption(inventoryOption);

  // 2. 库存价值图（堆积柱状图）
  const valueChart = echarts.init(document.getElementById('value-chart'));
  const valueOption = {
    title: {
      text: '每个仓库的库存价值',
      subtext: '单位：元',
    },
    tooltip: {
      trigger: 'axis',
    },
    legend: {
      data: getProductTypes(),
    },
    xAxis: {
      type: 'category',
      data: getWarehouseNames(),
    },
    yAxis: {
      type: 'value',
    },
    series: getInventoryValueSeries(),
  };
  valueChart.setOption(valueOption);

  // 3. 库存分布图（饼图）
  const distributionChart = echarts.init(document.getElementById('distribution-chart'));
  const distributionOption = {
    title: {
      text: '库存产品类型分布',
      subtext: '单位：件',
    },
    tooltip: {
      trigger: 'item',
      formatter: '{b}: {c} ({d}%)',
    },
    series: [
      {
        name: '产品类型分布',
        type: 'pie',
        radius: '55%',
        center: ['50%', '50%'],
        data: getProductTypeDistribution(),
      },
    ],
  };
  distributionChart.setOption(distributionOption);
};

// 获取所有仓库的名称
const getWarehouseNames = () => {
  return Array.from(new Set(storehouseData.value.map(item => item.name)));
};

// 获取每个仓库的库存量
const getWarehouseStockCounts = () => {
  return getWarehouseNames().map(warehouse => {
    return storehouseData.value
      .filter(item => item.name === warehouse)
      .reduce((total, item) => total + item.count, 0);
  });
};

// 获取所有的产品类型
const getProductTypes = () => {
  return Array.from(new Set(storehouseData.value.map(item => item.product_type)));
};

// 获取每个产品类型在各仓库的库存价值（堆积柱状图）
const getInventoryValueSeries = () => {
  const productTypes = getProductTypes();
  return productTypes.map(type => {
    return {
      name: type,
      type: 'bar',
      stack: '库存价值',
      data: getWarehouseNames().map(warehouse => {
        return storehouseData.value
          .filter(item => item.name === warehouse && item.product_type === type)
          .reduce((total, item) => total + (item.count * item.unit_price), 0);
      }),
    };
  });
};

// 获取每个产品类型在库存中的占比
const getProductTypeDistribution = () => {
  const productTypeCounts = storehouseData.value.reduce((acc, item) => {
    if (acc[item.product_type]) {
      acc[item.product_type] += item.count;
    } else {
      acc[item.product_type] = item.count;
    }
    return acc;
  }, {});
  return Object.entries(productTypeCounts).map(([type, count]) => ({
    name: type,
    value: count,
  }));
};

// 在组件加载时获取数据
onMounted(() => {
  fetchStorehouseData();
});
</script>

<template>
  <h1>仓库统计图</h1>
  <div class="chart-container">
    <!-- 库存量统计图 -->
    <div id="inventory-chart" class="chart-item"></div>

    <!-- 库存价值图 -->
    <div id="value-chart" class="chart-item"></div>

    <!-- 库存分布图 -->
    <div id="distribution-chart" class="chart-item"></div>
  </div>

  <el-table :data="storehouseData" style="width: 100%" border>
    <el-table-column label="仓库名称" prop="name" width="180"></el-table-column>
    <el-table-column label="产品编码" prop="product_code" width="180"></el-table-column>
    <el-table-column label="产品名称" prop="storehouse_address" width="180"></el-table-column>
    <el-table-column label="产品类型" prop="product_type" width="180"></el-table-column>
    <el-table-column label="单价" prop="unit_price" width="100"></el-table-column>
    <el-table-column label="数量" prop="count" width="100"></el-table-column>
    <el-table-column label="库存价值" width="100">
      <template #default="scope">
        <span>{{ scope.row.unit_price * scope.row.count }}</span>
      </template>
    </el-table-column>
  </el-table>
</template>

<style scoped>
.chart-container {
  display: flex;
  justify-content: space-between;
  margin-bottom: 20px;
}

.chart-item {
  width: 32%;
  height: 400px;
  margin: 10px 0;
}

.el-table {
  margin-top: 40px;
}
</style>
