<script setup>
import { ref, reactive, onMounted, watch } from 'vue';
import axios from 'axios';
import { ElMessage } from 'element-plus';


const purchaseForm = reactive({
  operation_type: "出库",
  orderId:  '' ,
  selectedOrderId: '',
  purchaseDate: '',
  warehouse: '666',
  staff: '',
});

const productTableData = ref([]);
const orderList = ref([]);
// const StaffList = ref([]);

// 表单引用
const purchaseFormRef = ref(null);

const calculateAmount = (Storehouses) => {
  // 计算总价格
  return Storehouses.reduce((total, product) => {
    return total + (product.unit_price * product.count);  // 确保使用正确的属性名称
  }, 0);
};

const yingshouForm = reactive({
  detail_id: 1,
  receivables: '',
  invoice_number: '',
  amount: 9, // 初始金额
  due_date: '',
  created_at: '',
});

// 提交表单
const submitForm = async () => {
  purchaseFormRef.value.validate(async () => {
    // 构建 payload，用于提交商品和仓库信息
    const payload = {
      Name: purchaseForm.warehouse,
      operation_type: purchaseForm.operation_type,
      Storehouses: productTableData.value.map(item => ({
        StorehouseAddress: item.name,
        ProductCode: item.product_code,
        ProductType: item.product_type,
        UnitPrice: item.unit_price,
        Count: item.count
      }))
    };

    // 构建应收信息
    const yingshou = {
      ...yingshouForm,
      receivables: "用户", // 动态替换为供应商名称
      amount: calculateAmount(productTableData.value), // 自动计算总价
    };

    try {
      // 提交仓库数据
      await axios.post('/File_Management/appOStorehouse_Table', payload);
      ElMessage.success('添加成功');

      // 提交应收明细数据
      try {
        await axios.post('/Finance/AR_Detail', yingshou);
        ElMessage.success('应收明细提交成功');
      } catch (error) {
        console.log("应收明细错误", error);
        ElMessage.error("应收明细提交失败");
      }
    } catch (error) {
      const message = error.response?.data?.message || '网络或服务端错误';
      ElMessage.error(`添加失败：${message}`);
    }
  });
};

// 查询订单商品信息
const fetchOrderProducts = async () => {
  try {
    const response = await axios.get(`/File_Management/order_sales_Table`, {
      params: { sales_order_id: purchaseForm.selectedOrderId },
    });
    const filteredData = response.data.filter(item => item.sales_order_id === purchaseForm.selectedOrderId);
    productTableData.value = filteredData;
    updateAmount(); // 商品数据更新后重新计算金额
  } catch (error) {
    console.error("查询订单商品信息失败", error);
  }
};

// 更新金额
const updateAmount = () => {
  yingshouForm.amount = calculateAmount(productTableData.value);
};

// 查询所有订单
const fetchOrders = async () => {
  try {
    const response = await axios.get('/File_Management/sales_orders');
    orderList.value = response.data;
    if (orderList.value.length > 0) {
      purchaseForm.selectedOrderId = orderList.value[0].sales_order_id;
      fetchOrderProducts();
    }
  } catch (error) {
    console.error("查询订单信息失败", error);
  }
};

const fetchStaffList = async () => {
  // 查询操作人员列表
  // 这里需要根据实际情况编写查询操作人员列表的逻辑
};

// 监听 selectedOrderId 的变化，并重新加载商品信息
watch(() => purchaseForm.selectedOrderId, () => {
  fetchOrderProducts();
});

// 自动生成订单编号
const generateOrderId = () => {
  const now = new Date();
  return (
    now.getMinutes().toString().padStart(2, '0') +
    now.getSeconds().toString().padStart(2, '0') +
    now.getMilliseconds().toString().padStart(3, '0')
  );
};

onMounted(() => {
  fetchOrders();
  fetchStaffList();
  purchaseForm.orderId = generateOrderId();
});
</script>

<template>
  <h1>出库表</h1>
  <el-form-item>
    <el-button type="primary" @click="submitForm">提交</el-button>
  </el-form-item>
  <el-form ref="purchaseFormRef" :model="purchaseForm" :rules="rules" label-width="120px">
    <el-form-item label="入库编号">
      <el-input v-model="purchaseForm.orderId" disabled></el-input>
    </el-form-item>
    <el-form-item label="订单编号">
    <el-select v-model="purchaseForm.selectedOrderId">
      <el-option
        v-for="order in orderList"
        :key="order.sales_order_id"
        :label="order.sales_order_id"
        :value="order.sales_order_id"
      />
    </el-select>
  </el-form-item>
    <el-form-item label="到货日期" prop="purchaseDate">
      <el-date-picker v-model="purchaseForm.purchaseDate" type="date" required></el-date-picker>
    </el-form-item>
    <el-form-item label="仓库" prop="warehouse">
        <el-input v-model="purchaseForm.warehouse" required></el-input>
  
    </el-form-item>
    <el-form-item label="操作人员" prop="staff">
      <!-- <el-select v-model="purchaseForm.staff" placeholder="请选择工作人员" required>
        <el-option v-for="staff in StaffList" :key="staff.id" :label="staff.name" :value="staff.id" />
      </el-select> -->
      <el-input v-model="purchaseForm.staff" required></el-input>

    </el-form-item>

    <el-table :data="productTableData" style="width: 100%" border>
      <el-table-column label="产品编码" prop="product_code">
        <template #default="scope">
          <el-input v-model="scope.row.product_code" readonly></el-input>
        </template>
      </el-table-column>
      <el-table-column label="产品名称" prop="name">
        <template #default="scope">
          <el-input v-model="scope.row.name" readonly></el-input>
        </template>
      </el-table-column>
      <el-table-column label="产品类型" prop="product_type">
        <template #default="scope">
          <el-input v-model="scope.row.product_type" readonly></el-input>
        </template>
      </el-table-column>
      <el-table-column label="单价" prop="unit_price">
        <template #default="scope">
          <el-input-number v-model="scope.row.unit_price" :min="0" disabled></el-input-number>
        </template>
      </el-table-column>
      <el-table-column label="数量" prop="count">
        <template #default="scope">
          <el-input-number v-model="scope.row.count" :min="0" disabled></el-input-number>
        </template>
      </el-table-column>
    </el-table>

    <!-- 显示总金额 -->
    <el-form-item label="总金额">
      <el-input v-model="yingshouForm.amount" disabled></el-input>
    </el-form-item>
  </el-form>
</template>
